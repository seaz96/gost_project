import { useUserStore } from '~/components/entities/user'
import { UserApi } from '../features/user/api/UserApi'

export default defineNuxtRouteMiddleware(async () => {
  const userStore = useUserStore()
  const userApi = UserApi()

  if (!userStore.currentUser) {
    try {
      const me = await userApi.getMe()

      if(me) {
        userStore.currentUser = me
      }
    } catch {
      return navigateTo('/sign', { redirectCode: 307 })
    }
  }
})
