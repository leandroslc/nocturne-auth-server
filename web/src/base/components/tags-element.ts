import { getProvider } from '../providers';
import {
  TagsElementProvider,
  TagsElementProviderFactoryOptions,
} from '../providers/types';

export default () => {
  const providerFactory = getProvider('tagsElementProvider');

  class TagsElement {
    private provider: TagsElementProvider;

    constructor(
      element: HTMLInputElement,
      options?: TagsElementProviderFactoryOptions,
    ) {
      this.provider = providerFactory(element, options);
    }

    disable() {
      this.provider.disable();
    }

    clean() {
      this.provider.clean();
    }

    disableAndClean() {
      this.disable();
      this.clean();
    }

    enable() {
      this.enable();
    }
  }

  return TagsElement;
};
