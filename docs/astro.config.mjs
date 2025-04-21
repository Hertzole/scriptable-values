// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import starlightLinksValidator from 'starlight-links-validator'

// https://astro.build/config
export default defineConfig({
	integrations: [
		starlight({
			plugins: [starlightLinksValidator()],
			title: 'Scriptable Values Documentation',
			social: [{ icon: 'github', label: 'GitHub', href: 'https://github.com/hertzole/scriptable-values' }],
			sidebar: [
				{
					label: 'Guides',
					autogenerate: { directory: 'guides' },
				},
				{
					label: 'Types',
					autogenerate: { directory: 'types' },
				}
			],
		}),
	],
});
