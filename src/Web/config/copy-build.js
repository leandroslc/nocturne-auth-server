import path from 'path';
import copy from 'rollup-plugin-copy';
import config from './config';

const build = ({ bundleName, inputs }) => {
  return {
    input: './index.js',
    plugins: config.getOutputs(bundleName, outputDir => {
      return copy({
        targets: Object.keys(inputs).map(destination => {
          return {
            src: inputs[destination],
            dest: path.join(outputDir, destination),
          };
        }),
      });
    }),
    watch: {
      skipWrite: true,
    },
  };
};

export default build;
