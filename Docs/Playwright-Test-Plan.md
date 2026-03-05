# Plan paso a paso: ejecutar tests de Playwright (Optimizely Demo CMS)

Guía para correr los tests de Playwright en este proyecto. Si ya conoces Cypress, aquí tienes el equivalente en Playwright.

---

## Equivalencias rápidas Cypress ↔ Playwright

| Concepto | Cypress | Playwright |
|----------|---------|------------|
| Carpeta de tests | `cypress-e2e/` | `tests-playwright/` |
| Entrar a la carpeta | `cd cypress-e2e` | `cd tests-playwright` |
| Instalar | `npm install` + (Cypress se instala con deps) | `npm install` + `npx playwright install chromium` |
| Abrir runner interactivo | `npm run cy:open` | `npm run test:ui` |
| Ejecutar en headless | `npm run cy:run` | `npm run test` o `npm run test:public` |
| Ver último reporte | (en la misma UI o artifacts) | `npm run report` |
| Actualizar snapshots visuales | (no aplica igual) | `npm run snap:update` |
| App debe estar corriendo | Sí (ej. `https://localhost:5000`) | Sí (`https://localhost:5000`) |

---

## Plan paso a paso

### Paso 1: Tener la app corriendo

1. En la **raíz del repo** (`C:\Users\Ignacio Tejera\Optimizely`), abre una terminal.
2. Ejecuta: **`dotnet run`**.
3. Espera a ver que escucha en `https://localhost:5000` (o la URL que use tu `launchSettings.json`).
4. Deja esta terminal abierta; la app debe seguir en ejecución mientras corren los tests.

*(En Cypress es igual: la app tiene que estar arriba antes de abrir el runner o hacer `cy:run`.)*

---

### Paso 2: Ir a la carpeta de Playwright

5. Abre **otra terminal** (para no cerrar la app).
6. Navega a la carpeta de tests:
   ```powershell
   cd C:\Users\Ignacio Tejera\Optimizely\tests-playwright
   ```
   O desde la raíz del repo:
   ```powershell
   cd tests-playwright
   ```

*(Equivalente a hacer `cd cypress-e2e` antes de cualquier comando de Cypress.)*

---

### Paso 3: Instalar dependencias (solo la primera vez o al cambiar de máquina)

7. Instala paquetes Node:
   ```powershell
   npm install
   ```
8. Instala el navegador que usa Playwright (Chromium):
   ```powershell
   npx playwright install chromium
   ```

*(En Cypress no sueles hacer “install browser” aparte; en Playwright sí, una vez por proyecto/máquina.)*

---

### Paso 4: Ejecutar los tests

Tienes varias opciones, según lo que quieras hacer.

#### Opción A: Solo tests del sitio público (recomendado al empezar)

9. En la terminal, dentro de `tests-playwright`, ejecuta:
   ```powershell
   npm run test:public
   ```
   - Correrán solo los specs de `tests/public/` (home, navegación, hero block, snapshot visual).
   - No hace falta login ni configuración extra.
   - Verás el resultado en la misma terminal (pass/fail).

*(Similar a ejecutar solo una carpeta de specs en Cypress.)*

#### Opción B: Modo UI (runner interactivo, como “cy:open”)

10. Ejecuta:
    ```powershell
    npm run test:ui
    ```
    - Se abre la **interfaz de Playwright** (ventana con lista de tests, timeline, etc.).
    - Puedes elegir qué specs correr, ver los pasos y el navegador.
    - Útil para depurar y ver qué hace cada test.

*(Equivalente a `npm run cy:open` en Cypress.)*

#### Opción C: Todos los tests

11. Ejecuta:
    ```powershell
    npm run test
    ```
    - Incluye tests públicos y los de CMS.
    - Los de CMS fallarán si no has hecho antes el login y guardado de sesión (paso 6 opcional).

---

### Paso 5: Ver el reporte HTML (después de una ejecución)

12. Tras correr tests (en headless), puedes abrir el último reporte con:
    ```powershell
    npm run report
    ```
    - Se abre en el navegador un reporte con resultados, trazas y capturas de fallos.

*(En Cypress el reporte suele estar en la misma UI o en `cypress/reports` si lo configuras; en Playwright es `npm run report`.)*

---

### Paso 6 (opcional): Tests del CMS (crear bloque, publicar, etc.)

Solo si quieres ejecutar los tests que tocan la UI del CMS:

13. Crea el archivo de sesión (login una vez):
    ```powershell
    $env:CMS_USER="admin"
    $env:CMS_PASS="TuPassword"
    npm run cms:login
    ```
    - Se ejecuta el test de login y se guarda el estado en `storage/auth.json`.
    - **Importante:** Usa el usuario y contraseña reales del CMS. Si el login falla, se guarda igual la sesión y el test "CMS loads after login" fallará después.
14. Luego puedes correr los tests de CMS:
    ```powershell
    npm run test:cms
    ```
    - Los specs en `tests/cms/` que usan `storage/auth.json` podrán ejecutarse (algunos están en scaffold y pueden estar en `test.skip`).

**Si "CMS loads after login" falla:** El test espera ver en la página texto como "Content", "Admin", "Edit", "Optimizely" o "CMS" tras cargar con la sesión guardada. Comprueba que (1) el usuario/contraseña son correctos y (2) al abrir `https://localhost:5000/episerver/cms` en el navegador con la app en marcha ves ese tipo de texto tras iniciar sesión. Para inspeccionar qué hay realmente en la página, ejecuta `npm run test:ui`, corre solo el test que falla y revisa el trace o la captura de pantalla del fallo.

---

### Paso 7: Si falla el test de “visual snapshot” (home-full.png)

15. La primera vez (o en una máquina nueva) el test de regresión visual puede fallar porque no existe la imagen de referencia.
16. Genera las referencias con:
    ```powershell
    npm run snap:update
    ```
    - Playwright guardará las capturas como baseline; las siguientes ejecuciones compararán contra ellas.

---

## Resumen del “camino correcto” (flujo típico)

1. **Terminal 1:** `dotnet run` (raíz del repo) → app en `https://localhost:5000`.
2. **Terminal 2:** `cd tests-playwright` → `npm install` (primera vez) → `npx playwright install chromium` (primera vez).
3. **Ejecutar:** `npm run test:public` (rápido y estable) o `npm run test:ui` (interactivo).
4. **Si hace falta:** `npm run report` para ver el último reporte; `npm run snap:update` si falla el snapshot visual.

---

## Dónde está cada cosa

| Qué | Dónde |
|-----|--------|
| Configuración (URL, timeouts, navegador) | `tests-playwright/playwright.config.ts` |
| Tests del sitio público | `tests-playwright/tests/public/*.spec.ts` |
| Tests del CMS | `tests-playwright/tests/cms/*.spec.ts` |
| Utilidades (estabilizar página, selectores) | `tests-playwright/test-utils/` |
| Reporte HTML (tras una ejecución) | Se genera en `tests-playwright/playwright-report/`; se abre con `npm run report` |

---

## Más detalle

- **tests-playwright/README.md** — Comandos, estructura, cómo completar los tests de CMS con codegen.
- **Docs/TESTING.md** — Estrategia de testing del proyecto (integración, Cypress, Playwright, Postman).
