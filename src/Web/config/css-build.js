import cssnano from 'cssnano';
import path from 'path';
import postcss from 'rollup-plugin-postcss';
import config from './config';

const build = ({ bundleName, input, watch, production }) => {
  const postCssPlugins = [];

  if (production) {
    postCssPlugins.push(cssnano());
  }

  const options = {
    input: input,
    output: config.getOutputs(bundleName, outputDir => {
      return {
        file: path.join(outputDir, config.getFileName(bundleName, 'css')),
      };
    }),
    plugins: [
      postcss({
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
