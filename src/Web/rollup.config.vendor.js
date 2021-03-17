import copy from './config/copy-build';
import css from './config/css-build';
import sass from './config/sass-build';

const bootstrapCSS = sass({
  bundleName: 'bootstrap',
  input: './vendor/bootstrap/scss/index.scss',
  production: true,
  watch: false,
});

const bootstrapIconsCSS = css({
  bundleName: 'bootstrap-icons',
  input: './node_modules/bootstrap-icons/font/bootstrap-icons.css',
  production: true,
});

const bootstrapIconsFonts = copy({
  bundleName: 'bootstrap-icons',
  inputs: {
    ['fonts']: './node_modules/bootstrap-icons/font/fonts/**/*',
  },
});

export default [
  bootstrapCSS,
  bootstrapIconsCSS,
  bootstrapIconsFonts,
];
