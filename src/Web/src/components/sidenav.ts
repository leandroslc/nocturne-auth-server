import constants from '../config/constants';

const EscapeKey = 'Escape';
const ToggleAttribute = constants.components.sidebar.toggleAttribute;
const ExpandedClass = constants.components.sidebar.expandedClass;
const BackdropClass = constants.components.sidebar.backdropClass;

class Sidenav {
  sidebarElement: HTMLElement;
  toggleElement: HTMLElement;
  backdropElement: HTMLElement;

  constructor(sidebar: HTMLElement, sidebarToggle: HTMLElement) {
    this.sidebarElement = sidebar;
    this.toggleElement = sidebarToggle;
    this.backdropElement = this._createBackdrop();
  }

  static findByToggle() {
    const toggleElement = document
      .querySelector(`[${ToggleAttribute}]`) as HTMLElement;

    if (!toggleElement) {
      return null;
    }

    const sidebarId = toggleElement.getAttribute(ToggleAttribute);

    const sidebarElement = sidebarId
      ? document.getElementById(sidebarId)
      : null;

    if (!sidebarElement) {
      return null;
    }

    return new Sidenav(sidebarElement, toggleElement);
  }

  initialize() {
    document.body.appendChild(this.backdropElement);

    this.toggleElement.addEventListener('click', this._createToggleOnClickEvent());
    this.backdropElement.addEventListener('click', this._createBackdropOnClickEvent());

    document.addEventListener('keyup', this._createDocumentOnKeyPressEvent());
  }

  _createBackdrop() {
    const backdrop = document.createElement('div');
    backdrop.classList.add(BackdropClass);

    return backdrop;
  }

  _isExpandend() {
    return this.sidebarElement.classList.contains(ExpandedClass);
  }

  _open() {
    this._setExpanded(this.sidebarElement, true);
    this._setExpanded(this.backdropElement, true);
  }

  _close() {
    this._setExpanded(this.sidebarElement, false);
    this._setExpanded(this.backdropElement, false);
  }

  _setExpanded(element: HTMLElement, isExpanded: boolean) {
    isExpanded
      ? element.classList.add(ExpandedClass)
      : element.classList.remove(ExpandedClass)
  }

  _createToggleOnClickEvent() {
    return (event: Event) => {
      event.preventDefault();

      this._isExpandend()
        ? this._close()
        : this._open();
    };
  }

  _createBackdropOnClickEvent() {
    return () => {
      this._close();
    };
  }

  _createDocumentOnKeyPressEvent() {
    return (event: KeyboardEvent) => {
      if (!this._isExpandend()) {
        return;
      }

      if (event.key === EscapeKey) {
        this._close();
      }
    }
  }
}

export default Sidenav;
