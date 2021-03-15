import path from 'path';

const Name = 'nocturne-auth-server';

const projects = {
  admin: path.resolve(__dirname, `../../src/Admin/wwwroot/lib/${Name}`),
  server: path.resolve(__dirname, `../../src/Server/wwwroot/lib/${Name}`),
};

const getOutputs = (outputMap) => {
  return Object.keys(projects).map(project => {
    return outputMap(projects[project]);
  });
};

const getFileName = (extension) => {
  return `${Name}.${extension}`;
};

const config = {
  getFileName,
  getOutputs,
};

export default config;
