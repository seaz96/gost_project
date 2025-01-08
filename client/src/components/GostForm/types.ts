import { z } from "zod";

export const gostFormSchema = z.object({
  designation: z.string().min(1, "Обязательное поле"),
  fullName: z.string().min(1, "Обязательное поле"),
  codeOks: z.string().min(1, "Обязательное поле"),
  activityField: z.string().min(1, "Обязательное поле"),
  acceptanceYear: z.number().min(1900).max(new Date().getFullYear()),
  commissionYear: z.number().min(1900).max(new Date().getFullYear() + 10),
  author: z.string().min(1, "Обязательное поле"),
  acceptedFirstTimeOrReplaced: z.string(),
  content: z.string().min(1, "Обязательное поле"),
  keyWords: z.string().min(1, "Обязательное поле"),
  applicationArea: z.string().min(1, "Обязательное поле"),
  adoptionLevel: z.enum(["International", "Foreign", "Regional", "Organizational", "National", "Interstate"]),
  documentText: z.string(),
  changes: z.string(),
  amendments: z.string(),
  status: z.enum(["Valid", "Canceled", "Replaced", "Inactive"]),
  harmonization: z.enum(["Harmonized", "Unharmonized", "Modified"]),
  references: z.array(z.string()),
});

export type GostFormValues = z.infer<typeof gostFormSchema>;
