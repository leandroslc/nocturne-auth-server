import { LoadingProvider } from '@/src/base/providers/types';

const provider: LoadingProvider = {
  toggleAttribute: 'loading',
  hideAttribute: 'loading-hide',
  isFormValid: (form) => {
    if (form.checkValidity && form.checkValidity() === false) {
      return false;
    }

    const $form = $(form);

    if ($form.valid && $form.valid() === false) {
      return false;
    }

    return true;
  },
};

export default provider;
