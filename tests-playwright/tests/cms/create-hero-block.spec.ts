import * as fs from "fs";
import * as path from "path";
import { test, expect } from "@playwright/test";

const authPath = path.join(process.cwd(), "storage", "auth.json");
const hasAuth = () => fs.existsSync(authPath);

/**
 * CMS: Create Hero Block.
 * Uses storageState from login.setup (run cms:login first).
 *
 * Flow: open CMS -> navigate to Home -> Hero Area -> Create new Hero Block -> fill Heading/Text -> Create/Publish.
 * Use codegen to capture real selectors: npx playwright codegen https://localhost:5000/episerver/cms
 */
test.describe("CMS - Create Hero Block", () => {
  test.use(hasAuth() ? { storageState: authPath } : {});

  test("CMS loads after login", async ({ page }) => {
    if (!hasAuth()) {
      test.skip(true, "Run npm run cms:login first to create storage/auth.json");
      return;
    }
    await page.goto("/episerver/cms");

    // After login we should see CMS shell (not the login form). Common Optimizely/EPiServer UI text.
    await expect(page.getByText(/Content|Admin|Dashboard|Edit|Optimizely|CMS/i).first()).toBeVisible({ timeout: 15_000 });
  });

  test.skip("Create Hero Block and fill Heading/Text", async ({ page }) => {
    await page.goto("/episerver/cms");

    // TODO: Use Playwright codegen to record:
    // 1) Open Home page in tree
    // 2) Hero Area -> Select content -> Create new -> Hero Block
    // 3) Fill Heading, Text -> Create / Publish
    await expect(page.getByText(/CMS/i)).toBeVisible();
  });
});
