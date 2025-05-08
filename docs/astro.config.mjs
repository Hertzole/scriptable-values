// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import starlightLinksValidator from 'starlight-links-validator'
import starlightSidebarTopics from 'starlight-sidebar-topics'

import sitemap from '@astrojs/sitemap';

// https://astro.build/config
export default defineConfig({
    site: 'https://www.hertzole.se',
    base: '/scriptable-values/',
    integrations: [starlight({
        plugins: [
            starlightLinksValidator({
                errorOnRelativeLinks: false,
              }),
            starlightSidebarTopics([
                {
                  label: 'Documentation',
                  link: '/guides/getting-started',
                  icon: 'open-book',
                  items: [
                    {
                        label: 'Guides',
                        autogenerate: { directory: 'guides' },
                    },
                    {
                        label: 'Types',
                        autogenerate: { directory: 'types' },
                    },
                    {
                        label: 'Components',
                        autogenerate: { directory: 'components' },
                    }
                  ],
                },
                {
                  label: 'Reference',
                  link: '/reference/',
                  icon: 'seti:html',
                  items: [
                    {
                        label: "Hertzole.ScriptableValues",
                        autogenerate: { directory: 'reference/Hertzole.ScriptableValues' },
                    },
                    {
                        label: "Hertzole.ScriptableValues.Editor",
                        autogenerate: { directory: 'reference/Hertzole.ScriptableValues.Editor' },
                    }
                  ],
                },
              ]),],
        title: 'Scriptable Values Documentation',
        logo: {
            src: './src/assets/sv_icon.webp',
            alt: 'Scriptable Values Logo',
        },
        social: [{ icon: 'github', label: 'GitHub', href: 'https://github.com/hertzole/scriptable-values' }],
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