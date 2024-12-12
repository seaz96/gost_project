export default async (request: any, opts?: any) => {
	const baseURL = import.meta.server ? (process.env.API_URL || 'http://localhost:3000/') : 'https://test.gost-storage.ru/'
	const fetch = $fetch.create({
		baseURL
	})

	const { cookie } = useRequestHeaders(['cookie'])

	return await fetch(
		request,
		{
			...opts,
			headers: { ...opts?.headers, cookie, Authorization: "Bearer " + localStorage.getItem("gost-storage-token") || "" }
		}
	)
		.catch((e) => {
			const error = e
			error.details = {}
			if (e.response && e.response._data) {
				error.message = e.response._data.error
				error.details = e.response._data.details
			}
			throw error
		})
}