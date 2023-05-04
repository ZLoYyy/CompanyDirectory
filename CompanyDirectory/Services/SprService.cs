using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDirectory.Services
{
    public class SprService
    {
        private readonly IRepository<Company> _companies;
        private readonly IRepository<Division> _divisions;
        private readonly IRepository<Employee> _employee;
        private readonly IRepository<Post> _posts;
        #region Конструкторы
        public SprService(IRepository<Company> companies) => _companies = companies;         
        public SprService(IRepository<Division> divisions) => _divisions = divisions;
        public SprService(IRepository<Employee> employee) => _employee = employee;
        public SprService(IRepository<Post> posts) => _posts = posts;
        #endregion

        public async Task<Company> AddCompany(Company company)
        {
            if (_companies == null)
                return null;

            var existingСompany = await _companies.Items.FirstOrDefaultAsync(c => c.Caption == company.Caption).ConfigureAwait(false);
            if (!(existingСompany is null)) return null;
            
            return await _companies.AddAsync(company);
        }
        public async Task UpdateCompany(Company company)
        {
            if (_companies == null)
                return;
         
            var existingСompany = await _companies.Items.FirstOrDefaultAsync(c => c.Id == company.Id).ConfigureAwait(false);
            if (existingСompany is null) return;

            await _companies.UpdateAsync(company);
        }

        public async Task RemoveCompany(Company company)
        {
            if (_companies == null)
                return;

            var existingСompany = await _companies.Items.FirstOrDefaultAsync(c => c.Id == company.Id).ConfigureAwait(false);
            if (existingСompany is null) return;

            await _companies.RemoveAsync(company.Id);
        }
    }
}
