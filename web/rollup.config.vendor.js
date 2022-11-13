import copy from './config/copy-build';
import css from './config/css-build';
import sass from './config/sass-build';
import typescript from './config/typescript-build';

const bootstrapCSS = sass({
  bundleName: 'bootstrap',
  input: './vendor/bootstrap/scss/index.scss',
  production: true,
  watch: false,
});

const bootstrapJS = copy({
  bundleName: 'bootstrap',
  inputs: {
    '': [
      './node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
      './node_modules/bootstrap/dist/js/bootstrap.min.js',
    ],
  },
});

const bootstrapIconsCSS = css({
  bundleName: 'bootstrap-icons',
  input: './node_modules/bootstrap-icons/font/bootstrap-icons.css',
  production: true,
});

const bootstrapIconsFonts = copy({
  bundleName: 'bootstrap-icons',
  inputs: {
    fonts: './node_modules/bootstrap-icons/font/fonts/**/*',
  },
});

const cookieConsent = copy({
  bundleName: 'cookieconsent',
  inputs: {
    '': [
      './node_modules/cookieconsent/build/cookieconsent.min.css',
      './node_modules/cookieconsent/build/cookieconsent.min.js',
    ],
  },
});

const customCookieConsentJS = typescript({
  bundleName: 'cookieconsent',
  name: 'cookieconsent-default',
  input: './vendor/cookieconsent/src/index.ts',
  production: true,
});

const fontAwesomeFreeCss = sass({
  bundleName: 'fontawesome',
  input: './vendor/fontawesome/scss/index.scss',
  production: true,
  watch: false,
});

const fontAwesomeFreeFonts = copy({
  bundleName: 'fontawesome',
  inputs: {
    webfonts: './node_modules/@fortawesome/fontawesome-free/webfonts/**/*',
  },
});

const imaskJS = copy({
  bundleName: 'imask',
  name: '',
  inputs: {
    '': './node_modules/imask/dist/imask.min.js',
  },
  production: true,
});

const jqueryJS = copy({
  bundleName: 'jquery',
  inputs: {
    '': [
      './node_modules/jquery/dist/jquery.min.js',
      './node_modules/jquery/dist/jquery.slim.min.js',
    ],
  },
});

const jqueryValidationJS = copy({
  bundleName: 'jquery-validation',
  inputs: {
    '': [
      './node_modules/jquery-validation/dist/jquery.validate.min.js',
      './node_modules/jquery-validation/dist/additional-methods.min.js',
    ],
  },
});

const jqueryValidationUnobtrusiveJS = copy({
  bundleName: 'jquery-validation-unobtrusive',
  inputs: {
    '': './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
  },
});

const qrCodeJS = copy({
  bundleName: 'qrcode',
  inputs: {
    '': ['./vendor/qrcode/qrcode.min.js'],
  },
});

const tagifyCSS = sass({
  bundleName: 'tagify',
  input: './vendor/tagify/scss/index.scss',
  production: true,
  watch: false,
});

const tagifyJS = copy({
  bundleName: 'tagify',
  inputs: {
    '': ['./node_modules/@yaireo/tagify/dist/tagify.min.js'],
  },
});

export default [
  bootstrapCSS,
  bootstrapJS,
  bootstrapIconsCSS,
  bootstrapIconsFonts,
  cookieConsent,
  customCookieConsentJS,
  fontAwesomeFreeCss,
  fontAwesomeFreeFonts,
  imaskJS,
  jqueryJS,
  jqueryValidationJS,
  jqueryValidationUnobtrusiveJS,
  qrCodeJS,
  tagifyCSS,
  tagifyJS,
];
