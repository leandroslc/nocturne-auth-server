import autoprefixer from 'autoprefixer';
import path from 'path';
import postcss from 'rollup-plugin-postcss';
import config from './config';

const postCssPlugins = [
  autoprefixer(),
];

const css = {
  input: './scss/index.scss',
  output: config.getOutputs(outputDir => {
    return {
      file: path.join(outputDir, config.getFileName('css')),
    };
  }),
  plugins: [
    postcss({
      use: ['sass'],
      extract: true,
      modules: false,
      plugins: postCssPlugins,
    }),
  ],
  watch: {
    include: ['./scss/**/*.scss'],
  },
};

export default css;
