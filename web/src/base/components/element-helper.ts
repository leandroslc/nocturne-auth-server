import { getProvider } from '../providers';

export default () => {
  const provider = getProvider('elementHelperProvider');

  const togglePassword = (element: HTMLInputElement, button: HTMLElement) => {
    const isPassword = element.type === 'password';
    const isPasswordElements = button.querySelectorAll<HTMLElement>(
      `[${provider.inputPasswordElement}]`,
    );

    isPasswordElements.forEach((e) => {
      const elementShouldBeVisibleWhenPasswordEnabled =
        e.getAttribute(provider.inputPasswordElement) === 'true';

      if (elementShouldBeVisibleWhenPasswordEnabled) {
        e.style.display = isPassword ? 'none' : '';
      } else {
        e.style.display = isPassword ? '' : 'none';
      }
    });

    element.setAttribute('type', isPassword ? 'text' : 'password');
  };

  const addTogglePasswordEvent = (
    element: HTMLInputElement,
    button: HTMLElement,
  ) => {
    button.addEventListener('click', () => {
      togglePassword(element, button);
    });
  };

  const copyText = (element: HTMLInputElement) => {
    const isPassword = element.type === 'password';

    element.select();
    element.setSelectionRange(0, 99999);

    if (isPassword) {
      element.setAttribute('type', 'text');
    }

    document.execCommand('copy');

    if (isPassword) {
      element.setAttribute('type', 'password');
    }
  };

  const getRequiredElementById = (id: string) => {
    const element = document.getElementById(id);

    if (element) {
      return element;
    }

    throw new Error(`Element "${id}" not found`);
  };

  const queryRequiredElementBySelector = <T extends HTMLElement>(
    selector: string,
    parentElement: ParentNode = document,
  ) => {
    const element = parentElement.querySelector<T>(selector);

    if (element) {
      return element;
    }

    throw new Error(`Element with selector "${selector}" not found`);
  };

  const autoFocus = (parentElement: ParentNode = document) => {
    const elementToFocus = parentElement.querySelector<HTMLElement>(
      provider.autofocusAttribute,
    );

    if (elementToFocus && elementToFocus.focus) {
      elementToFocus.focus();
    }
  };

  const clearFormInputs = (parentElement: ParentNode = document) => {
    parentElement
      .querySelectorAll<HTMLInputElement>('input, textarea, select')
      .forEach((element) => {
        // eslint-disable-next-line no-param-reassign
        element.value = '';
      });
  };

  return {
    addTogglePasswordEvent,
    copyText,
    getRequiredElementById,
    queryRequiredElementBySelector,
    autoFocus,
    clearFormInputs,
  };
};
