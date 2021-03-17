import path from 'path';
import typescript from '@rollup/plugin-typescript';
import config from './config';

const build = ({ bundleName, input, watch }) => {
  const options = {
    input: input,
    output: config.getOutputs(bundleName, outputDir => {
      return {
        file: path.join(outputDir, config.getFileName(bundleName, 'js')),
        format: 'iife',
      };
    }),
    plugins: [
      typescript(),
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
