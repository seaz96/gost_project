import type { User } from "~/components/shared/types/user"

export const useUserStore = defineStore("user", () => {
	const currentUser = ref<User>()

	return {
			currentUser
	}
})