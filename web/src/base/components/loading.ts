import {
  setDisabledAll,
  setEnabledAll,
  show,
  hide,
  tryGetElementById,
} from '../utils/elements';
import { getProvider } from '../providers';

const hasAtLeastOneElement = (elements: NodeListOf<Element>) => {
  return elements.length > 0;
};

export default () => {
  const provider = getProvider('loadingProvider');

  class Loading {
    private form: HTMLFormElement;

    private loading?: HTMLElement;

    private elementToHide?: HTMLElement;

    private submits?: NodeListOf<Element> | null;

    constructor(form: HTMLFormElement) {
      this.form = form;
    }

    static initializeAll() {
      this.findByToggle().forEach((loading) => {
        loading.initialize();
        loading.attachSubmitEvent();
      });
    }

    static findByToggle() {
      const foundForms = document.querySelectorAll(
        `[${provider.toggleAttribute}]`,
      );

      const loadings: Loading[] = [];

      foundForms.forEach((foundForm) =>
        loadings.push(new Loading(foundForm as HTMLFormElement)),
      );

      return loadings;
    }

    static create(form: HTMLFormElement) {
      const loading = new Loading(form);

      loading.initialize();

      return loading;
    }

    initialize() {
      const loadingElementId = this.form.getAttribute(provider.toggleAttribute);

      if (loadingElementId) {
        this.loading = tryGetElementById(loadingElementId);
      }

      const elementToHideId = this.form.getAttribute(provider.hideAttribute);

      if (elementToHideId) {
        this.elementToHide = tryGetElementById(elementToHideId);
      }

      this.submits = this.form.querySelectorAll('[type="submit"]');
    }

    attachSubmitEvent() {
      this.form.addEventListener('submit', (event) => {
        if (
          provider.isFormValid(event.currentTarget as HTMLFormElement) === false
        ) {
          return;
        }

        this.start();
      });
    }

    start() {
      if (this.submits && hasAtLeastOneElement(this.submits)) {
        setDisabledAll(this.submits);
      }

      if (this.loading) {
        show(this.loading);
      }

      if (this.elementToHide) {
        hide(this.elementToHide);
      }
    }

    stop() {
      if (this.submits && hasAtLeastOneElement(this.submits)) {
        setEnabledAll(this.submits);
      }

      if (this.loading) {
        hide(this.loading);
      }

      if (this.elementToHide) {
        show(this.elementToHide);
      }
    }
  }

  return Loading;
};
