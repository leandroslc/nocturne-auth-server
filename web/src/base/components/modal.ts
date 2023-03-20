import { getProvider } from '../providers';
import { ModalProvider } from '../providers/types';

export default () => {
  const providerFactory = getProvider('modalProvider');

  class Modal {
    private provider: ModalProvider;

    private element: HTMLElement;

    private content: HTMLElement;

    constructor(id: string) {
      this.element = window.ElementHelper.getRequiredElementById(id);
      this.provider = providerFactory(this.element);
      this.content = window.ElementHelper.queryRequiredElementBySelector(
        this.provider.contentSelector,
      );

      this.provider.addOnModalClosedEvent((event) =>
        window.ElementHelper.clearFormInputs(event.target as ParentNode),
      );
      this.provider.addOnModalOpenedEvent((event) =>
        window.ElementHelper.autoFocus(event.target as ParentNode),
      );
    }

    show() {
      this.provider.open();
    }

    close() {
      this.provider.close();
    }

    setContent(html: string) {
      this.content.innerHTML = html;
    }

    querySelector(selector: string) {
      return this.content.querySelector(selector);
    }
  }

  return Modal;
};
