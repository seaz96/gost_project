import fetch from "~/components/shared/libs/fetch"

export const GostApi = () => {
  const loadGosts = async (lastId: number) => {
    return await fetch('/api/docs/all?' + new URLSearchParams({ lastId: lastId.toString() }).toString())
  }

  return {
    loadGosts
  }
}