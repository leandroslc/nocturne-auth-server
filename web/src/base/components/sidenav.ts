import { getProvider } from '../providers';

const EscapeKey = 'Escape';

export default () => {
  const provider = getProvider('sidenavProvider');

  class Sidenav {
    private sidebarElement: HTMLElement;

    private toggleElement: HTMLElement;

    private backdropElement: HTMLElement;

    constructor(sidebar: HTMLElement, sidebarToggle: HTMLElement) {
      this.sidebarElement = sidebar;
      this.toggleElement = sidebarToggle;
      this.backdropElement = this.createBackdrop();
    }

    static findByToggle() {
      const toggleElement = document.querySelector(
        `[${provider.toggleAttribute}]`,
      ) as HTMLElement;

      if (!toggleElement) {
        return null;
      }

      const sidebarId = toggleElement.getAttribute(provider.toggleAttribute);

      const sidebarElement = sidebarId
        ? document.getElementById(sidebarId)
        : null;

      if (!sidebarElement) {
        return null;
      }

      return new Sidenav(sidebarElement, toggleElement);
    }

    public initialize() {
      document.body.appendChild(this.backdropElement);

      this.toggleElement.addEventListener(
        'click',
        this.createToggleOnClickEvent(),
      );

      this.backdropElement.addEventListener(
        'click',
        this.createBackdropOnClickEvent(),
      );

      document.addEventListener('keyup', this.createDocumentOnKeyPressEvent());
    }

    private createBackdrop() {
      const backdrop = document.createElement('div');
      backdrop.classList.add(provider.backdropClass);

      return backdrop;
    }

    private isExpanded() {
      return this.sidebarElement.classList.contains(provider.expandedClass);
    }

    private open() {
      this.setExpanded(this.sidebarElement, true);
      this.setExpanded(this.backdropElement, true);
    }

    private close() {
      this.setExpanded(this.sidebarElement, false);
      this.setExpanded(this.backdropElement, false);
    }

    private setExpanded(element: HTMLElement, isExpanded: boolean) {
      if (isExpanded) {
        element.classList.add(provider.expandedClass);
      } else {
        element.classList.remove(provider.expandedClass);
      }
    }

    private createToggleOnClickEvent() {
      return (event: Event) => {
        event.preventDefault();

        if (this.isExpanded()) {
          this.close();
        } else {
          this.open();
        }
      };
    }

    private createBackdropOnClickEvent() {
      return () => {
        this.close();
      };
    }

    private createDocumentOnKeyPressEvent() {
      return (event: KeyboardEvent) => {
        if (!this.isExpanded()) {
          return;
        }

        if (event.key === EscapeKey) {
          this.close();
        }
      };
    }
  }

  return Sidenav;
};
