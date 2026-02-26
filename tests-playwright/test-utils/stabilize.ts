import { Page } from "@playwright/test";

/**
 * Stabilize page before assertions or screenshots:
 * - Wait for load states (dom + networkidle)
 * - Disable animations/transitions to reduce flakiness
 * - Scroll to top
 */
export async function stabilizePage(page: Page): Promise<void> {
  await page.waitForLoadState("domcontentloaded");
  await page.waitForLoadState("networkidle");

  await page.addStyleTag({
    content: `
      *, *::before, *::after {
        transition: none !important;
        animation: none !important;
        caret-color: transparent !important;
      }
    `,
  });

  await page.evaluate(() => window.scrollTo(0, 0));
}
