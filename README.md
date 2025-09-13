# E-Commerce Product Listing

A full-stack e-commerce product listing application with ASP.NET Core backend and Vue.js frontend.

## Quick Start

1. Backend: `dotnet run --project ProductApi`
2. Frontend: `npm install && npm run dev`

## Features

- Product listing with grid layout
- Search by name
- Filter by category and price range
- Pagination with customizable page size
- Product details with related products
- Mobile-responsive design

## API Endpoints

- `GET /api/products` - Get products with search, filters, and pagination
- `GET /api/products/{id}/related` - Get related products

## Technology Stack

- Backend: ASP.NET Core Web API
- Frontend: Vue.js with Router
- UI: CSS Grid and Flexbox for responsive layout

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Run Unit Tests with [Vitest](https://vitest.dev/)

```sh
npm run test:unit
```

### Run End-to-End Tests with [Playwright](https://playwright.dev)

```sh
# Install browsers for the first run
npx playwright install

# When testing on CI, must build the project first
npm run build

# Runs the end-to-end tests
npm run test:e2e
# Runs the tests only on Chromium
npm run test:e2e -- --project=chromium
# Runs the tests of a specific file
npm run test:e2e -- tests/example.spec.ts
# Runs the tests in debug mode
npm run test:e2e -- --debug
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```
