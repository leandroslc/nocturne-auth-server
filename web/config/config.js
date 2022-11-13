import path from 'path';

const projectName = 'nocturne-auth-server';

const buildOutputs = {
  admin: path.resolve(__dirname, '../src/Admin/wwwroot/dist'),
  server: path.resolve(__dirname, '../src/Server/wwwroot/dist'),
};

const getOutputs = ({ name, output }, outputMap) => {
  const allBuildOutputs = Object.keys(buildOutputs);

  const outputs = !output
    ? allBuildOutputs
    : allBuildOutputs.filter((p) => p === output);

  return outputs.map((outputName) => {
    const outputDir = path.join(buildOutputs[outputName], name);

    return outputMap(outputDir);
  });
};

const getFileName = ({ name, extension, production }) => {
  return `${name}.${production ? 'min.' : ''}${extension}`;
};

const config = {
  projectName,
  getFileName,
  getOutputs,
};

export default config;
