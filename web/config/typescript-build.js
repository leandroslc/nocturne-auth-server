import path from 'path';
import terser from '@rollup/plugin-terser';
import typescript from '@rollup/plugin-typescript';
import config from './config';

const build = ({ bundleName, name, output, input, watch, production }) => {
  const plugins = [typescript()];

  if (production) {
    plugins.push(terser());
  }

  const options = {
    input,
    output: config.getOutputs({ name: bundleName, output }, (outputDir) => {
      return {
        file: path.join(
          outputDir,
          config.getFileName({
            name: name || bundleName,
            extension: 'js',
            production,
          }),
        ),
        format: 'iife',
      };
    }),
    plugins,
  };

  if (watch) {
    options.watch = {
      include: [watch],
    };
  }

  return options;
};

export default build;
