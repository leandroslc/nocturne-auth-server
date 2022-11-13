import autoprefixer from 'autoprefixer';
import cssnano from 'cssnano';
import path from 'path';
import postcss from 'rollup-plugin-postcss';
import config from './config';

const build = ({ bundleName, name, output, input, watch, production }) => {
  const postCssPlugins = [autoprefixer()];

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
            name: name || bundleName,
            extension: 'css',
            production,
          }),
        ),
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
