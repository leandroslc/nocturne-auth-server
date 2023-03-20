import './providers';
import elementHelper from './base/components/element-helper';
import loading from './base/components/loading';
import modal from './base/components/modal';
import responseHelper from './base/components/response-helper';
import sidenav from './base/components/sidenav';
import tagsElement from './base/components/tags-element';
import validator from './base/components/validator';

declare global {
  interface Window {
    ElementHelper: ReturnType<typeof elementHelper>;
    Loading: ReturnType<typeof loading>;
    Modal: ReturnType<typeof modal>;
    ResponseHelper: ReturnType<typeof responseHelper>;
    TagifyElement: ReturnType<typeof tagsElement>;
    Validator: ReturnType<typeof validator>;
  }
}

window.ElementHelper = elementHelper();
window.Loading = loading();
window.Modal = modal();
window.ResponseHelper = responseHelper();
window.TagifyElement = tagsElement();
window.Validator = validator();

window.Loading.initializeAll();

sidenav().findByToggle()?.initialize();
