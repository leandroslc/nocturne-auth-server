import { setProviders } from '@/src/base/providers';
import elementHelperProvider from './element-helper-provider';
import loadingProvider from './loading-provider';
import modalProvider from './modal-provider';
import sidenavProvider from './sidenav-provider';
import tagsElementProvider from './tags-element-provider';
import validatorProvider from './validator-provider';

setProviders({
  elementHelperProvider,
  loadingProvider,
  modalProvider,
  sidenavProvider,
  tagsElementProvider,
  validatorProvider,
});
