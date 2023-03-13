
<h1 align="center">
  Impromptu
</h1>

<p align="center">
  This is a very simplistic improvised compilation of web design assets to help create a consistent design used be the <a href="https://github.com/leandroslc/nocturne-auth-server">Nocturne Auth Server</a>.
</p>

## Build assets
- Run `npm run vendor` to generate the vendor assets.
- Run `npm run build` to generate the production version.


## Customization
For now, the only way to use and customize _Impromptu_ is by cloning the project.


### Customizing styles
Most of the styles (including vendor's) are compiled using [Sass](https://sass-lang.com). There are variables that can be modified for quick customization, like colors. For _Impromptu_, these variables are placed in `scss/core/_vars.scss`. For vendors, they are usually placed in `_variables.scss` files.

**Note:** It is highly encouraged to change the default colors.


## Included assets
Beside _Impromptu_ itself, this project includes the following assets:

Name                           | Version
:----------------------------- | :------
bootstrap                      | 4.6
bootstrap-icons                | 1.4
cookieconsent                  | 3.1
fontawesome-free               | 5.15
imask                          | 6.1
jquery                         | 3.6
jquery-validation              | 1.19
jquery-validation-unobtrusive  | 3.2
