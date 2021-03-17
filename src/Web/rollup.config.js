import config from './config/config';
import sass from './config/sass-build';
import typescript from './config/typescript-build';

const css = sass({
  bundleName: config.projectName,
  input: './scss/index.scss',
  watch: './scss/**/*.scss',
});

const js = typescript({
  bundleName: config.projectName,
  input: './src/index.ts',
  watch: './src/**/*.ts',
});

export default [
  css,
  js,
];
