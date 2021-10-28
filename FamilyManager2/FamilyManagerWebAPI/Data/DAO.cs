using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace FamilyManagerWebAPI.Data {
    public class DAO : IDAO {
        private FileContext file;
        
        public DAO() {
            file = new FileContext();
        }

        public async Task<IList<string>> GetEyeColorsAsync() {
            throw new System.NotImplementedException();
        }

        public async Task<IList<string>> GetHairColorsAsync() {
            throw new System.NotImplementedException();
        }


        public async Task<IList<Family>> GetFamiliesAsync() {
            List<Family> families = new List<Family>(file.Families);
            return families;
        }

        public async Task<Family> GetFamilyAsync(string streetName, int houseNumber) {
            Family fam = file.Families.FirstOrDefault(f => f.StreetName.Equals(streetName) && f.HouseNumber == houseNumber);
            if (fam == null) {
                throw new NullReferenceException("No family was found with these street name and house number");
            }
            return fam;
        }

        public async Task<Family> AddFamilyAsync(Family family) {
            Family fam = file.Families.FirstOrDefault(f =>
                f.StreetName.Equals(family.StreetName) && f.HouseNumber == family.HouseNumber);
            if (fam != null) {
                throw new InvalidDataException("This family already exists");
            }
            file.Families.Add(family);
            file.SaveDataToFile();
            return family;
        }

        public async Task<Family> RemoveFamilyAsync(string streetName, int houseNumber) {
            Family fam = await GetFamilyAsync(streetName, houseNumber);
            file.Families.Remove(fam);
            file.SaveDataToFile();
            return fam;
        }

        public async Task<Family> UpdateFamilyAsync(Family family) {
            Family fam = await GetFamilyAsync(family.StreetName, family.HouseNumber);
            fam.StreetName = family.StreetName;
            fam.HouseNumber = family.HouseNumber;
            fam.Pets = family.Pets;
            fam.Children = family.Children;
            fam.Adults = family.Adults;
            file.SaveDataToFile();
            return fam;
        }

        public async Task<Adult> AddAdultAsync(string streetName, int houseNumber, Adult adult) {
            foreach (Family fam in file.Families) {
                if (fam.StreetName.Equals(streetName) && fam.HouseNumber == houseNumber) {
                    fam.Adults.Add(adult);
                }
            }
            file.SaveDataToFile();
            return adult;
        }
        public async Task<IList<Adult>> GetAdultsByFamilyAsync(string streetName, int houseNumber) {
            return file.Families.First(f => f.StreetName.Equals(streetName) && f.HouseNumber == houseNumber).Adults;
        }
        
        public async Task<Adult> GetAdultAsync(int id) {
            IList<Adult> adults = await GetAdultsAsync();
            Adult adult = adults.FirstOrDefault(adult => adult.Id == id);
            if (adult == null) {
                throw new NullReferenceException("There is no adult with such id");
            }
            return adult;
        }

        public async Task DeleteAdultAsync(int id) {
            foreach (var family in file.Families) {
                foreach (var adult in family.Adults) {
                    if (adult.Id == id) 
                        family.Adults.Remove(adult);
                    else {
                        throw new NullReferenceException("Cannot delete an adult that does not exist with that id");
                    }
                }
            }
            file.SaveDataToFile();
        }

        public async Task<Adult> UpdateAdultAsync(int id, Adult a) {
            Adult adult = await GetAdultAsync(id);
            adult.FirstName = a.FirstName;
            adult.LastName = a.LastName;
            adult.HairColor = a.HairColor;
            adult.JobTitle = a.JobTitle;
            adult.Age = a.Age;
            adult.Height = a.Height;
            adult.Weight = a.Weight;
            adult.EyeColor = a.EyeColor;
            file.SaveDataToFile();
            return adult;
        }

        public async Task<IList<Adult>> GetAdultsAsync() {
            IList<Adult> adults = new List<Adult>();
            foreach (var family in file.Families) {
                foreach (Adult adult in family.Adults) {
                    adults.Add(adult);
                }
            }
            return adults;
        }

        public async Task<IList<Child>> GetChildrenAsync() {
            IList<Child> children = new List<Child>();
            foreach (Family f in file.Families) {
                foreach (Child c in f.Children) {
                    children.Add(c);
                }
            }
            return children;
        }

        public async Task<IList<Child>> GetChildrenAsync(string streetName, int houseNumber) {
            Family family = file.Families.FirstOrDefault(f => f.StreetName.Equals(streetName) && f.HouseNumber == houseNumber);
            if (family == null) throw new NullReferenceException("No such family found");
            return family.Children;
        }

        public async Task<Child> GetChildAsync(int childId) {
            Child child = (await GetChildrenAsync()).FirstOrDefault(c => c.Id == childId);
            if (child == null) throw new NullReferenceException("No such child found");
            return child;
        }

        public async Task<Child> AddChildAsync(string streetName, int houseNumber, Child child) {
            Family family = file.Families.FirstOrDefault(f => f.StreetName.Equals(streetName) && f.HouseNumber == houseNumber);
            if (family == null) throw new NullReferenceException("No such child found");
            int current = family.Children.Max(c => c.Id);
            child.Id = current + 1;
            family.Children.Add(child);
            file.SaveDataToFile();
            return child;
        }

        public async Task<Child> UpdateChildAsync(int childId, Child updatedChild) {
            Child child = await GetChildAsync(childId);
            child.Age = updatedChild.Age;
            child.Height = updatedChild.Height;
            child.Sex = updatedChild.Sex;
            child.Weight = updatedChild.Weight;
            child.EyeColor = updatedChild.EyeColor;
            child.FirstName = updatedChild.FirstName;
            child.HairColor = updatedChild.HairColor;
            child.LastName = updatedChild.LastName;
            child.Interests = updatedChild.Interests;
            child.Pets = child.Pets;
            file.SaveDataToFile();
            return child;
        }

        public async Task DeleteChildAsync(int childId) {
            Child child = await GetChildAsync(childId);
            file.People.Remove(child);
            file.SaveDataToFile();
        }

        public async Task<Pet> GetPetAsync(int petId) {
            return file.Pets.First(p => petId == p.Id);
        }

        public async Task<IList<Pet>> GetPetsAsync(string street, int number) {
            return file.Families.First(f => f.StreetName.Equals(street) && f.HouseNumber == number).Pets;
        }

        public async Task<Pet> AddPetAsync(Pet pet, string street, int number) {
            file.Families.First(f => f.StreetName.Equals(street) && f.HouseNumber == number).Pets.Add(pet);
            return pet;
        }

        public async Task<Pet> AddPetAsync(Pet pet, string street, int number, int childId) {
            file.Families.First(f => f.StreetName.Equals(street) && f.HouseNumber == number).Children.First(c => c.Id == childId).Pets.Add(pet);
            file.SaveDataToFile();
            return pet;
        }

        public async Task<Pet> UpdatePetAsync(int id, Pet pet) {
            Pet p = file.Pets.First(p => p.Id == id);
            p.Age = pet.Age;
            p.Name = pet.Name;
            p.Species = pet.Species;
            file.SaveDataToFile();
            return p;
        }

        public async Task RemovePetAsync(int petId) {
            foreach (var pet in file.Pets) {
                if (pet.Id == petId)
                    file.Pets.Remove(pet);
            }
            file.SaveDataToFile();
        }
    }
}