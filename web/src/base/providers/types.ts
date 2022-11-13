export type ElementHelperProvider = {
  autofocusAttribute: string;
  inputPasswordElement: string;
};

export type LoadingProvider = {
  toggleAttribute: string;
  hideAttribute: string;
  isFormValid: (form: HTMLFormElement) => boolean;
};

export type ModalProvider = {
  contentSelector: string;
  addOnModalOpenedEvent(listener: () => void): void;
  addOnModalClosedEvent(listener: () => void): void;
  open(): void;
  close(): void;
};

export type ModalProviderFactory = (element: HTMLElement) => ModalProvider;

export type SidenavProvider = {
  expandedClass: string;
  backdropClass: string;
  toggleAttribute: string;
};

export type ValidatorProvider = {
  attach: (form: HTMLFormElement) => void;
  isValid: (form: HTMLFormElement) => boolean;
};

export type TagsElementProvider = {
  clean: () => void;
  disable: () => void;
  enable: () => void;
};

export type TagsElementProviderFactoryOptions = {
  allowedEntries?: string[];
};

export type TagsElementProviderFactory = (
  element: HTMLInputElement,
  options?: TagsElementProviderFactoryOptions,
) => TagsElementProvider;

export type Providers = {
  elementHelperProvider: ElementHelperProvider;
  loadingProvider: LoadingProvider;
  modalProvider: ModalProviderFactory;
  sidenavProvider: SidenavProvider;
  tagsElementProvider: TagsElementProviderFactory;
  validatorProvider: ValidatorProvider;
};
