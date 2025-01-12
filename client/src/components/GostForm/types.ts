import { z } from "zod";

export const gostFormSchema = z.object({
	designation: z.string().min(1, "Обязательное поле"),
	fullName: z.string().nullable(),
	codeOks: z.string().nullable(),
	activityField: z.string().nullable(),
	acceptanceYear: z
		.any()
		.transform((val) => (val === "" ? null : Number(val)))
		.nullable()
		.refine((val) => !val || (val >= 1000 && val <= new Date().getFullYear()), {
			message: `Год принятия должен быть числом между 1000 и ${new Date().getFullYear()}`,
		}),
	commissionYear: z
		.any()
		.transform((val) => (val === "" ? null : Number(val)))
		.nullable()
		.refine((val) => val === null || (val >= 1000 && val <= new Date().getFullYear() + 1000), {
			message: `Год принятия должен быть числом между 1000 и ${new Date().getFullYear() + 10}`,
		}),
	author: z.string().nullable(),
	acceptedFirstTimeOrReplaced: z.string().nullable(),
	content: z.string().nullable(),
	keyWords: z.string().nullable(),
	applicationArea: z.string().nullable(),
	adoptionLevel: z
		.enum(["International", "Foreign", "Regional", "Organizational", "National", "Interstate"])
		.nullable(),
	documentText: z.string().nullable(),
	changes: z.string().nullable(),
	amendments: z.string().nullable(),
	status: z.enum(["Valid", "Canceled", "Replaced", "Inactive"]).nullable(),
	harmonization: z.enum(["Harmonized", "Unharmonized", "Modified"]).nullable(),
	references: z.array(z.string()),
	file: z.any().nullable(),
});

export type GostFormValues = z.infer<typeof gostFormSchema>;