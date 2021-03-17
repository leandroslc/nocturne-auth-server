import autoprefixer from 'autoprefixer';
import path from 'path';
import postcss from 'rollup-plugin-postcss';
import config from './config';

const postCssPlugins = [
  autoprefixer(),
];

const build = ({ bundleName, input, watch }) => {
  const options = {
    input: input,
    output: config.getOutputs(bundleName, outputDir => {
      return {
        file: path.join(outputDir, config.getFileName(bundleName, 'css')),
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
  };

  if (watch) {
    options.watch = {
      include: [watch],
    };
  }

  return options;
};

export default build;