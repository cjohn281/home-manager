/**
 * jquery-poster.js - A simple jQuery plugin for handling loading states
 */
(function ($) {
    'use strict';

    var poster = {
        overlayClass: 'loading-overlay',
        spinnerClass: 'loading-spinner',
        timeout: null,
        delay: 300,

        initialize: function () {
            if ($('.' + this.overlayClass).length === 0) {
                $('body').append(
                    `<div class="${this.overlayClass}" style="display: none;">
                        <div class="${this.spinnerClass}">
                            <div class="spinner-border text-primary" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    </div>`
                );
            }

            // Add required CSS
            if ($('#poster-styles').length === 0) {
                $('head').append(
                    `<style id="poster-styles">
                        .${this.overlayClass} {
                            position: fixed;
                            top: 0;
                            left: 0;
                            width: 100%;
                            height: 100%;
                            background: rgba(255, 255, 255, 0.7);
                            z-index: 9999;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                        }
                        .${this.spinnerClass} {
                            text-align: center;
                        }
                    </style>`
                );
            }
        },

        on: function (message) {
            this.initialize();
            clearTimeout(this.timeout);
            this.timeout = setTimeout(() => {
                $('.' + this.overlayClass).fadeIn(200);
            }, this.delay);
        },

        off: function () {
            clearTimeout(this.timeout);
            $('.' + this.overlayClass).fadeOut(200);
        }
    };

    // Attach to window
    window.poster = poster;

})(jQuery);