import path from 'path';
import typescript from '@rollup/plugin-typescript';
import config from './config';

const js = {
  input: './src/index.ts',
  output: config.getOutputs(outputDir => {
    return {
      file: path.join(outputDir, config.getFileName('js')),
      format: 'cjs',
    };
  }),
  plugins: [
    typescript(),
  ],
  watch: {
    include: ['./src/**/*.ts'],
  },
};

export default js;
