import { ModalProviderFactory } from '@/src/base/providers/types';

const provider: ModalProviderFactory = (element) => {
  const $modal = $(element);

  return {
    contentSelector: '.modal-content',
    addOnModalClosedEvent: (listener) => {
      $modal.on('hidden.bs.modal', listener as () => void);
    },
    addOnModalOpenedEvent: (listener) => {
      $modal.on('shown.bs.modal', listener as () => void);
    },
    close: () => {
      $modal.modal('hide');
    },
    open: () => {
      $modal.modal('show');
    },
  };
};

export default provider;
