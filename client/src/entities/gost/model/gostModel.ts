export type Gost = {
    "docId": number,
    "primary": GostFields,
    "actual": GostFields,
    "references":
    {
        "docId": number,
        "designation": string,
        "status": 0 | 1 | 2,
    }[]
}

export type GostFields = {
    "designation": string,
    "fullName": string,
    "codeOKS": string,
    "activityField": string,
    "acceptanceYear": string,
    "commissionYear": string,
    "author": string,
    "acceptedFirstTimeOrReplaced": string,
    "content": string,
    "keyWords": string,
    "applicationArea": string,
    "adoptionLevel": number,
    "documentText": string,
    "changes": string,
    "amendments": string,
    "status": number,
    "harmonization": number,
    "isPrimary": true,
}

export type GostViews = {
    designation: string,
    docId: number,
    fullName: string,
    views: number,
}

export type GostChanges = {
    count: number,
    stats: {
        designation: string,
        docId: number,
        fullName: string,
        action: 'Create' | 'Update',
        date: number,
    }[]
}

export type GostGeneralInfo = {
    id: number,
    designation: string,
    codeOKS: string,
    fullName: string,
    applicationArea: string
}

export const Statuses = [
    'Действующий',
    'Отменён',
    'Заменён'
]

export const Harmonization = [
    'Негармонизированный',
    'Модифицированный',
    'Гармонизированный'
]

export const AdoptionLevel = [
    'Международный',
    'Иностранный',
    'Региональный',
    'Организационный',
    'Национальный',
    'Межгосударственный'
]