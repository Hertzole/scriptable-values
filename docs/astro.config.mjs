// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import starlightLinksValidator from 'starlight-links-validator'

import sitemap from '@astrojs/sitemap';

// https://astro.build/config
export default defineConfig({
    site: 'https://www.hertzole.se',
    base: '/scriptable-values/',
    integrations: [starlight({
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
        customCss: [
            './src/styles/custom.css'
        ],
        head: [
            {
                tag: 'meta',
                attrs: {
                    property: 'og:type',
                    content: 'website'
                }
            },
            {
                tag: 'meta',
                attrs: {
                    property: 'og:image',
                    content: 'https://repository-images.githubusercontent.com/615674071/68d86ae3-50ef-4b55-bc57-613b797a7e1e'
                }
            }
        ]
		}), sitemap()],
});