# encoding: utf-8

module Jekyll
  class SortedCategoriesBuilder < Generator

    safe true
    priority :high

    def generate(site)
      site.config['sorted_categories'] = site.categories.sort { |a,b| b[1].size <=> a[1].size }
      site.config['sorted_tags'] = site.tags.sort { |a,b| b[1].size <=> a[1].size }
    end

  end
end