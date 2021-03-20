(function () {
    'use strict';

    var constants = {
        components: {
            sidebar: {
                expandedClass: 'is-expanded',
                backdropClass: 'im-sidenav-backdrop',
                toggleAttribute: 'im-sidenav-toggle',
            },
        },
    };

    var EscapeKey = 'Escape';
    var ToggleAttribute = constants.components.sidebar.toggleAttribute;
    var ExpandedClass = constants.components.sidebar.expandedClass;
    var BackdropClass = constants.components.sidebar.backdropClass;
    var Sidenav = /** @class */ (function () {
        function Sidenav(sidebar, sidebarToggle) {
            this.sidebarElement = sidebar;
            this.toggleElement = sidebarToggle;
            this.backdropElement = this._createBackdrop();
        }
        Sidenav.findByToggle = function () {
            var toggleElement = document
                .querySelector("[" + ToggleAttribute + "]");
            if (!toggleElement) {
                return null;
            }
            var sidebarId = toggleElement.getAttribute(ToggleAttribute);
            var sidebarElement = sidebarId
                ? document.getElementById(sidebarId)
                : null;
            if (!sidebarElement) {
                return null;
            }
            return new Sidenav(sidebarElement, toggleElement);
        };
        Sidenav.prototype.initialize = function () {
            document.body.appendChild(this.backdropElement);
            this.toggleElement.addEventListener('click', this._createToggleOnClickEvent());
            this.backdropElement.addEventListener('click', this._createBackdropOnClickEvent());
            document.addEventListener('keyup', this._createDocumentOnKeyPressEvent());
        };
        Sidenav.prototype._createBackdrop = function () {
            var backdrop = document.createElement('div');
            backdrop.classList.add(BackdropClass);
            return backdrop;
        };
        Sidenav.prototype._isExpandend = function () {
            return this.sidebarElement.classList.contains(ExpandedClass);
        };
        Sidenav.prototype._open = function () {
            this._setExpanded(this.sidebarElement, true);
            this._setExpanded(this.backdropElement, true);
        };
        Sidenav.prototype._close = function () {
            this._setExpanded(this.sidebarElement, false);
            this._setExpanded(this.backdropElement, false);
        };
        Sidenav.prototype._setExpanded = function (element, isExpanded) {
            isExpanded
                ? element.classList.add(ExpandedClass)
                : element.classList.remove(ExpandedClass);
        };
        Sidenav.prototype._createToggleOnClickEvent = function () {
            var _this = this;
            return function (event) {
                event.preventDefault();
                _this._isExpandend()
                    ? _this._close()
                    : _this._open();
            };
        };
        Sidenav.prototype._createBackdropOnClickEvent = function () {
            var _this = this;
            return function () {
                _this._close();
            };
        };
        Sidenav.prototype._createDocumentOnKeyPressEvent = function () {
            var _this = this;
            return function (event) {
                if (!_this._isExpandend()) {
                    return;
                }
                if (event.key === EscapeKey) {
                    _this._close();
                }
            };
        };
        return Sidenav;
    }());

    document.addEventListener('DOMContentLoaded', function () {
        var _a;
        (_a = Sidenav.findByToggle()) === null || _a === void 0 ? void 0 : _a.initialize();
    });

}());
