module.exports = {
  root: true,
  rules: {
    'vue/multi-word-component-names': 'off',
    'vue/max-attributes-per-line': [
      1,
      {
        singleline: {
          max: 1
        },
        multiline: {
          max: 1
        }
      }
    ],
    'max-len': [
      1,
      {
        code: 120,
        tabWidth: 2,
        ignoreUrls: true,
        ignoreTrailingComments: true,
        ignoreTemplateLiterals: true,
        ignoreStrings: false
      }
    ],
    'vue/no-multiple-template-root': 'off',
    '@typescript-eslint/no-unused-vars': 'off',
    'n/no-callback-literal': 'off'
  },
  extends: [
    'eslint:recommended',
    'plugin:vue/essential',
    '@nuxtjs/eslint-config-typescript'
  ]
}
