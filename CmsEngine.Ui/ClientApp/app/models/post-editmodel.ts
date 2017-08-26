import { DocumentStatus } from './enums';

export class PostEditModel {
  public id: number;
  public vanityId: string;
  public title: string;
  public slug: string;
  public description: string;
  public documentContent: string;
  public author: string;
  public status: DocumentStatus;
}
