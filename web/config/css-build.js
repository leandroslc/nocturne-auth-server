import cssnano from 'cssnano';
import path from 'path';
import postcss from 'rollup-plugin-postcss';
import config from './config';

const build = ({ bundleName, output, input, watch, production }) => {
  const postCssPlugins = [];

  if (production) {
    postCssPlugins.push(cssnano());
  }

  const options = {
    input,
    output: config.getOutputs({ name: bundleName, output }, (outputDir) => {
      return {
        file: path.join(
          outputDir,
          config.getFileName({
            name: bundleName,
            extension: 'css',
            production,
          }),
        ),
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
