import { TemplateRef } from "@angular/core";

export interface Interview {
  id: number;
  title: string;
  company: string;
  date: string;
  status: 'scheduled' | 'completed' | 'cancelled';
  interviewer: string;
  duration: string;
  feedback?: string;
  score?: number;
}

export interface JobPost {
  id: number;
  title: string;
  company: string;
  location: string;
  type: string;
  salary: string;
  experience: string;
  description: string;
  requirements: string[];
  posted: string;
  deadline: string;
}
export interface MetricCard {
  title: string;
  value: string | number;
  change: string;
  icon: string;
}

export interface SkillItem {
  name: string;
  category: string;
  growth: string;
}

export interface ChartData {
  month: string;
  amount: number;
}

export interface TableItem {
  name: string;
  email: string;
  status: 'Active' | 'Pending' | 'Inactive';
  interviews: number;
}

export interface CandidateItem extends TableItem {}
export interface HRItem extends TableItem {}
export interface InterviewerItem extends TableItem {}

