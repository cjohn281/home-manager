using home_manager.Areas.BudgetManager.DTOs;
using home_manager.Areas.BudgetManager.Models;
using home_manager.Areas.BudgetManager.Repositories;
using home_manager.Areas.BudgetManager.ViewModels;
using home_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace home_manager.Areas.BudgetManager.Controllers
{
    [Area("BudgetManager")]
    [Authorize]
    public class LedgerController : Controller
    {
        private readonly IBudgetManagerRepository _repository;

        public LedgerController(IBudgetManagerRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("Ledger/{month?}/{year?}")]
        public async Task<IActionResult> Index(int? month = null, int? year = null)
        {
            var model = new AvailableLedgerDropdown_VModel();

            if (month == null || year == null)
            {
                model.SelectedLedger = (await _repository.GetLatestAvailableLedger());
            }
            else
            {
                model.SelectedLedger = (month.Value, year.Value);
            }

            model.LedgerMonths = (await _repository.GetAvailableLedgerMonths()).ToList();
            model.LedgerYears = (await _repository.GetAvailableLedgerYears()).ToList();

            return View(model);
        }


        public async Task<IActionResult> GetLedgerTable(int month, int year, int editableId)
        {
            var model = new LedgerDTO();

            model.Items = (await _repository.GetLedgerItemsByMonth(month, year)).ToList();

            var prevMonth = month == 1 ? 12 : --month;
            var prevYear = month == 12 ? --year : year;

            var balances = (await _repository.GetEndingBalances(prevMonth, prevYear));
            model.PreviousCheckingEndingBalance = balances.Item1;
            model.PreviousSavingsEndingBalance = balances.Item2;
            model.EditableItemId = editableId;

            return PartialView("_LedgerTable", model);
        }


        public async Task<IActionResult> UpdateLedgerItem([FromBody] LedgerItemDTO dto)
        {
            try
            {

                var Id = dto.Id;
                var Amount = dto.Amount;
                var IsPaid = dto.IsPaid;
                var Date = dto.Date;

                // Save the incidental item using the repository
                var result = await _repository.UpdateLedgerItem(Id, Amount, IsPaid, Date);

                if (result)
                    return Json(new { success = true });
                else
                    return BadRequest("Failed to save the ledger item.");
            }
            catch (Exception ex)
            {
                // Log the error as needed
                return BadRequest($"Error saving incidental expense: {ex.Message}");
            }
        }


        public async Task<IActionResult> CreateNewLedger()
        {

            var latestLedger = await _repository.GetLatestAvailableLedger();

            // Get new ledger month and year
            int originalMonth = latestLedger.Item1;
            int originalYear = latestLedger.Item2;

            int newMonth = originalMonth < 12 ? originalMonth + 1 : 1;
            int newYear = newMonth == 1 ? originalYear + 1 : originalYear;


            // Create new ledger and incidental records
            await _repository.InsertNewLedger(newMonth, newYear);

            var incomeDetailItems = (await _repository.GetIncomeDetails()).ToList();

            if (incomeDetailItems.Count > 0)
            {
                List<IncomeLedgerItem> incomeLedgerItems = await BuildIncomeLedgerItemsAsync(incomeDetailItems, newMonth, newYear);

                foreach (var item in incomeLedgerItems)
                {
                    await _repository.UpdateIncomeLedgerItem(item);
                }
            }

            var recurringSavingsItems = (await _repository.GetRecurringSavingsTransferDetails()).ToList();

            if (recurringSavingsItems.Count > 0)
            {
                List<SavingsLedgerItem> savingsLedgerItems = await BuildSavingsLedgerItemsAsync(recurringSavingsItems, newMonth, newYear);

                foreach (var item in savingsLedgerItems)
                {
                    await _repository.UpdateSavingsLedgerItem(item);
                }
            }

            return Json(new
            {
                success = true,
                month = newMonth,
                year = newYear
            });


        }


        private async Task<List<IncomeLedgerItem>> BuildIncomeLedgerItemsAsync(IEnumerable<IncomeDetail> incomeDetailItems, int newMonth, int newYear)
        {
            var incomeLedgerItems = new List<IncomeLedgerItem>();

            foreach (var item in incomeDetailItems)
            {
                if (item.LookupValue_lvlId == 29 || item.LookupValue_lvlId == 30)
                {
                    string newDateStr = $"{newMonth:D2}/{(item.PayDate1?.ToString("D2") ?? "01")}/{newYear:D4}";

                    if (DateTime.TryParse(newDateStr, out DateTime newDate))
                    {
                        incomeLedgerItems.Add(new IncomeLedgerItem
                        {
                            Person_prnId = item.Person_prnId,
                            Date = newDate,
                            Amount = item.DefaultAmount,
                            Month = newMonth,
                            Year = newYear
                        });
                    }
                }

                if (item.LookupValue_lvlId == 30)
                {
                    int lastDay = DateTime.DaysInMonth(newYear, newMonth);
                    string newDateStr = $"{newMonth:D2}/{(item.PayDate2.HasValue ? Math.Min(item.PayDate2.Value, lastDay).ToString("D2") : "01")}/{newYear:D4}";

                    if (DateTime.TryParse(newDateStr, out DateTime newDate))
                    {
                        incomeLedgerItems.Add(new IncomeLedgerItem
                        {
                            Person_prnId = item.Person_prnId,
                            Date = newDate,
                            Amount = item.DefaultAmount,
                            Month = newMonth,
                            Year = newYear
                        });
                    }
                }

                if (item.LookupValue_lvlId == 31)
                {
                    DateTime latestPayDate = await _repository.GetLatestPayDate(item.Person_prnId);
                    DateTime newPayDate = latestPayDate.Year == 1970
                        ? new DateTime(newYear, newMonth, 1)
                        : latestPayDate.AddDays(14);

                    while (newPayDate.Year < newYear || (newPayDate.Year == newYear && newPayDate.Month <= newMonth))
                    {
                        if (newPayDate.Month == newMonth && newPayDate.Year == newYear)
                        {
                            incomeLedgerItems.Add(new IncomeLedgerItem
                            {
                                Person_prnId = item.Person_prnId,
                                Date = newPayDate,
                                Amount = item.DefaultAmount,
                                Month = newMonth,
                                Year = newYear
                            });
                        }

                        newPayDate = newPayDate.AddDays(14);
                    }
                }
            }

            return incomeLedgerItems;
        }

        private async Task<List<SavingsLedgerItem>> BuildSavingsLedgerItemsAsync(IEnumerable<RecurringSavingsTransferDetail> savingsDetailItems, int newMonth, int newYear)
        {
            var savingsLedgerItems = new List<SavingsLedgerItem>();

            foreach (var item in savingsDetailItems)
            {
                if (item.Frequency_lvlId == 29 || item.Frequency_lvlId == 30)
                {
                    string newDateStr = $"{newMonth:D2}/{(item.Date1?.ToString("D2") ?? "01")}/{newYear:D4}";

                    if (DateTime.TryParse(newDateStr, out DateTime newDate))
                    {
                        savingsLedgerItems.Add(new SavingsLedgerItem
                        {
                            Lookupvalue_lvlId = item.TransferType_lvlId,
                            Date = newDate,
                            Amount = item.Amount,
                            RecurringDetailId = item.Id,
                            Month = newMonth,
                            Year = newYear
                        });
                    }
                }

                if (item.Frequency_lvlId == 30)
                {
                    int lastDay = DateTime.DaysInMonth(newYear, newMonth);
                    string newDateStr = $"{newMonth:D2}/{(item.Date2.HasValue ? Math.Min(item.Date2.Value, lastDay).ToString("D2") : "01")}/{newYear:D4}";

                    if (DateTime.TryParse(newDateStr, out DateTime newDate))
                    {
                        savingsLedgerItems.Add(new SavingsLedgerItem
                        {
                            Lookupvalue_lvlId = item.TransferType_lvlId,
                            Date = newDate,
                            Amount = item.Amount,
                            RecurringDetailId = item.Id,
                            Month = newMonth,
                            Year = newYear
                        });
                    }
                }

                if (item.Frequency_lvlId == 31)
                {
                    DateTime latestDate = await _repository.GetLatestRecurringSavingsDate(item.Id);
                    DateTime newDate = latestDate.Year == 1970
                        ? new DateTime(newYear, newMonth, 1)
                        : latestDate.AddDays(14);

                    while (newDate.Year < newYear || (newDate.Year == newYear && newDate.Month <= newMonth))
                    {
                        if (newDate.Month == newMonth && newDate.Year == newYear)
                        {
                            savingsLedgerItems.Add(new SavingsLedgerItem
                            {
                                Lookupvalue_lvlId = item.TransferType_lvlId,
                                Date = newDate,
                                Amount = item.Amount,
                                RecurringDetailId = item.Id,
                                Month = newMonth,
                                Year = newYear
                            });
                        }

                        newDate = newDate.AddDays(14);
                    }
                }
            }

            return savingsLedgerItems;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
