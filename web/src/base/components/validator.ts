import { getProvider } from '../providers';

export default () => {
  const provider = getProvider('validatorProvider');

  return {
    attach: provider.attach,
    isValid: provider.isValid,
  };
};
