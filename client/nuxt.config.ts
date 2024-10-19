export default defineNuxtConfig({
  devtools: { enabled: false },
  css: ['~/components/shared/assets/css/style.scss'],
  ssr: false,
  modules: ['@pinia/nuxt'],

  dir: {
    pages: './components/pages',
    plugins: './components/shared/plugins',
    assets: './components/shared/assets'
  },

  hooks: {
    'vite:extendConfig' (viteInlineConfig, _env) {
      viteInlineConfig.server = {
        ...viteInlineConfig.server,
        hmr: {
          protocol: 'ws',
          port: 24678
        }
      }
    }
  },

  vite: {
    ssr: {
      noExternal: ['vueuc']
    },
    build: {
      cssCodeSplit: false,
      copyPublicDir: false
    }
  },

  compatibilityDate: '2024-10-15'
})