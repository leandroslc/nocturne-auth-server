import path from 'path';

const projectName = 'nocturne-auth-server';

const projects = {
  admin: path.resolve(__dirname, `../../src/Admin/wwwroot/lib`),
  server: path.resolve(__dirname, `../../src/Server/wwwroot/lib`),
};

const getOutputs = (name, outputMap) => {
  return Object.keys(projects).map(project => {
    const outputDir = path.join(projects[project], name);

    return outputMap(outputDir);
  });
};

const getFileName = (name, extension) => {
  return `${name}.${extension}`;
};

const getFileNameByExtension = (extension) => {
  return getFileName(projectName, extension);
};

const config = {
  projectName,
  getFileName,
  getFileNameByExtension,
  getOutputs,
};

export default config;
