export type documentStatus = "Valid" | "Canceled" | "Replaced" | "Inactive";
export type harmonization = "Unharmonized" | "Modified" | "Harmonized";
export type adoptionLevel = "International" | "Foreign" | "Regional" | "Organizational" | "National" | "Interstate";

export type GostSearchParams = {
	Text: string;
	SearchFilters: {
		CodeOks: string | null;
		AcceptanceYear: number | null;
		CommissionYear: number | null;
		Author: string | null;
		AcceptedFirstTimeOrReplaced: string | null;
		KeyWords: string | null;
		AdoptionLevel: adoptionLevel | null;
		Status: documentStatus | null;
		Harmonization: harmonization | null;
		Changes: string | null;
		Amendments: string | null;
	};
	Limit: number;
	Offset: number;
};

export type GostAddModel = GostRequestModel & {
	file?: File;
};

export type GostRequestModel = {
	designation: string;
	fullName?: string;
	codeOks?: string;
	activityField?: string;
	acceptanceYear?: number;
	commissionYear?: number;
	author?: string;
	acceptedFirstTimeOrReplaced?: string;
	content?: string;
	keyWords?: string;
	applicationArea?: string;
	adoptionLevel?: adoptionLevel;
	documentText?: string;
	changes?: string;
	amendments?: string;
	status?: documentStatus;
	harmonization?: harmonization;
	references?: string[];
};

export type GostFetchModel = {
	id: number;
	status: documentStatus;
	primary: GostFieldsWithId;
	actual: GostFieldsWithId;
	references: GostReference[];
};

export type GostFieldsWithId = {
	id: number;
	designation: string;
	fullName: string;
	codeOks: string;
	activityField: string;
	acceptanceYear: number;
	commissionYear: number;
	author: string;
	acceptedFirstTimeOrReplaced: string;
	content: string;
	keyWords: string;
	applicationArea: string;
	adoptionLevel: adoptionLevel;
	documentText: string;
	changes: string;
	amendments: string;
	harmonization: harmonization;
	lastEditTime: string;
};

export type GostReference = {
	id: number;
	designation: string;
	status: documentStatus;
	actualFieldId: number;
	primaryFieldId: number;
};

export type GostViews = {
	designation: string;
	id: number;
	fullName: string;
	views: number;
};

export type GostChanges = {
	designation: string;
	fullName: string;
	documentId: number;
	userId: number;
	action: "Create" | "Update";
	orgBranch: string | null;
	date: number;
};

export type GostViewInfo = {
	id: number;
	codeOks: string;
	designation: string;
	fullName: string;
	applicationArea: string;
	relevanceMark: number;
};

export const IntToStatus = {
	0: "Valid",
	1: "Canceled",
	2: "Replaced",
	3: "Inactive",
};

export const StatusToRu = {
	Valid: "Действующий",
	Canceled: "Отменён",
	Replaced: "Заменён",
	Inactive: "Неактивный",
};

export const AdoptionLevelToRu = {
	International: "Международный",
	Foreign: "Иностранный",
	Regional: "Региональный",
	Organizational: "Организационный",
	National: "Национальный",
	Interstate: "Межгосударственный",
};

export const HarmonizationToRu = {
	Unharmonized: "Негармонизированный",
	Modified: "Модифицированный",
	Harmonized: "Гармонизированный",
};