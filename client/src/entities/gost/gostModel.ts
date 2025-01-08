export type status = "Valid" | "Canceled" | "Replaced" | "Inactive";
export type harmonization = "Unharmonized" | "Modified" | "Harmonized";
export type adoptionLevel = "International" | "Foreign" | "Regional" | "Organizational" | "National" | "Interstate";

export type GostSearchParams = {
	text: string;
	SearchFilters: {
		CodeOks: string;
		AcceptanceYear: number;
		CommissionYear: number;
		Author: string;
		AcceptedFirstTimeOrReplaced: string;
		KeyWords: string;
		AdoptionLevel: adoptionLevel | null;
		Status: status | null;
		Harmonization: harmonization | null;
	};
	Limit: number;
	Offset: number;
};

export type GostRequestModel = {
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
	status: status;
	harmonization: harmonization;
	references: string[];
};

export type GostFetchModel = {
	docId: number;
	status: status;
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
	docId: number;
	lastEditTime: string;
};

export type GostReference = {
	id: number;
	designation: string;
	status: status;
	actualFieldId: number;
	primaryFieldId: number;
};

export type GostViews = {
	designation: string;
	docId: number;
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
