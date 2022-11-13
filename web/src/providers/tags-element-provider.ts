import { setDisabled, setEnabled } from '@/src/base/utils/elements';
import { TagsElementProviderFactory } from '@/src/base/providers/types';

declare global {
  class Tagify {
    constructor(element: Element, options: unknown);

    public setReadonly(readonly: boolean): void;

    public removeAllTags(): void;
  }
}

const provider: TagsElementProviderFactory = (element, options) => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const defaultOptions: any = {
    delimiters: ',| ',
    keepInvalidTags: false,
    autoComplete: {
      enabled: false,
    },
    editTags: {
      clicks: 1,
      keepInvalid: false,
    },
    originalInputValueFormat: (values: { value: string }[]) => {
      return values.map((item) => item.value).join(' ');
    },
  };

  if (options && options.allowedEntries) {
    defaultOptions.enforceWhitelist = true;
    defaultOptions.whitelist = options.allowedEntries;
  }

  const tagify = new Tagify(element, defaultOptions);

  // Store and restore active element functions are workarounds to prevent
  // tagify from stealing focus from other elements.
  let currentActiveElement: HTMLElement | null;

  const storeActiveElement = () => {
    currentActiveElement = document.activeElement as HTMLElement;
  };

  const restoreActiveElement = () => {
    if (currentActiveElement) {
      currentActiveElement.focus();
    }
  };

  return {
    clean: () => {
      storeActiveElement();

      tagify.removeAllTags();

      // eslint-disable-next-line no-param-reassign
      element.value = '';

      restoreActiveElement();
    },
    disable: () => {
      storeActiveElement();

      tagify.setReadonly(true);
      setDisabled(element);

      restoreActiveElement();
    },
    enable: () => {
      storeActiveElement();

      tagify.setReadonly(false);
      setEnabled(element);

      restoreActiveElement();
    },
  };
};

export default provider;
